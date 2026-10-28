import { useState, useEffect, useRef } from 'react';
import api from '../services/api';

export function useProduction() {
    const [operatorEmail, setOperatorEmail] = useState('');
    const [activeTab, setActiveTab] = useState('form');

    const [productionOrders, setProductionOrders] = useState([]);
    const [selectedOrderObj, setSelectedOrderObj] = useState(null);
    const [selectedOrderCode, setSelectedOrderCode] = useState('');
    const [quantity, setQuantity] = useState('');
    const [selectedMaterial, setSelectedMaterial] = useState('');
    const [history, setHistory] = useState([]);

    // Estados do Modal customizado
    const [modalState, setModalState] = useState({ isOpen: false, type: 'success', message: '' });

    const [elapsedTime, setElapsedTime] = useState(0);
    const timerRef = useRef(null);

    useEffect(() => {
        fetchOrders();
    }, []);

    useEffect(() => {
        fetchHistory();
    }, [operatorEmail]);

    useEffect(() => {
        if (selectedOrderObj) {
            setElapsedTime(0);
            timerRef.current = setInterval(() => {
                setElapsedTime(prev => prev + 1);
            }, 1000);
        } else {
            if (timerRef.current) clearInterval(timerRef.current);
            setElapsedTime(0);
        }

        return () => {
            if (timerRef.current) clearInterval(timerRef.current);
        };
    }, [selectedOrderObj]);

    const fetchOrders = async () => {
        try {
            const response = await api.get('/orders/GetOrders');
            const responseData = response.data;
            const list = Array.isArray(responseData)
                ? responseData
                : (responseData.orders || responseData.$values || []);
            setProductionOrders(list);
        } catch (error) {
            console.error("Erro ao buscar ordens:", error);
            setProductionOrders([]);
        }
    };

    const fetchHistory = async () => {
        if (!operatorEmail.trim()) {
            setHistory([]);
            return;
        }

        try {
            const response = await api.get(`/orders/GetProduction?email=${encodeURIComponent(operatorEmail.trim())}`);
            const responseData = response.data;
            const list = Array.isArray(responseData)
                ? responseData
                : (responseData.productions || responseData.data || responseData.$values || []);
            setHistory(list);
        } catch (error) {
            console.error("Erro ao buscar histórico:", error);
            setHistory([]);
        }
    };

    const handleOrderChange = (e) => {
        const code = e.target.value;
        setSelectedOrderCode(code);
        const found = productionOrders.find(o => o.order === code);
        setSelectedOrderObj(found || null);
        setSelectedMaterial('');
    };

    const requiredCycleTime = selectedOrderObj?.cycleTime || 0;
    const isCycleTimeValid = elapsedTime >= requiredCycleTime;
    const isFormValid = operatorEmail.trim() !== '' && selectedOrderObj !== null && isCycleTimeValid && quantity > 0 && selectedMaterial !== '';

    const submitProduction = async (e) => {
        e.preventDefault();
        if (!isFormValid) return;

        const now = new Date();
        const productionDate = now.toISOString().split('T')[0];
        const productionTime = now.toTimeString().split(' ')[0];

        const payload = {
            email: operatorEmail,
            order: selectedOrderCode,
            productionDate,
            productionTime,
            quantity: Number(quantity),
            materialCode: selectedMaterial,
            cycleTime: elapsedTime
        };

        try {
            const res = await api.post('/orders/SetProduction', payload);
            const data = res.data;

            const resType = data?.type || data?.Type;
            const resMessage = data?.message || data?.Message || data?.description || data?.Description;

            if (data && (resType === 'E' || resType === 'Error' || data.status >= 400 || data.hasError || data.HasError)) {
                setModalState({
                    isOpen: true,
                    type: 'error',
                    message: resMessage || 'Falha na validação do back-end'
                });
                return;
            }

            setModalState({
                isOpen: true,
                type: 'success',
                message: 'Apontamento enviado com sucesso!'
            });

            setSelectedOrderCode('');
            setSelectedOrderObj(null);
            setQuantity('');
            setSelectedMaterial('');
            setElapsedTime(0);

            fetchHistory();
            setActiveTab('history');
        } catch (err) {
            console.error("Erro ao enviar apontamento:", err);
            const errorMsg = err.response?.data?.message || err.response?.data?.Message || err.response?.data?.description || err.response?.data?.Description || err.message || 'Erro ao enviar o apontamento.';
            setModalState({
                isOpen: true,
                type: 'error',
                message: errorMsg
            });
        }
    };

    const closeModal = () => {
        setModalState(prev => ({ ...prev, isOpen: false }));
    };

    return {
        operatorEmail, setOperatorEmail,
        activeTab, setActiveTab,
        productionOrders,
        selectedOrderObj,
        selectedOrderCode,
        quantity, setQuantity,
        selectedMaterial, setSelectedMaterial,
        history,
        elapsedTime,
        requiredCycleTime,
        isCycleTimeValid,
        isFormValid,
        modalState,
        closeModal,
        handleOrderChange,
        submitProduction
    };
}
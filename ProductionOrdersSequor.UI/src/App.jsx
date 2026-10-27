import React, { useState, useEffect, useRef } from 'react';
import api from './services/api';
import './App.css';

function App() {
  const [email, setEmail] = useState('');
  const [orders, setOrders] = useState([]);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [selectedMaterial, setSelectedMaterial] = useState('');
  const [quantity, setQuantity] = useState('');
  
  // Controle do Tempo de Ciclo
  const [startTime, setStartTime] = useState(null);
  const [elapsedTime, setElapsedTime] = useState(0);
  const timerRef = useRef(null);

  // Status de Retorno
  const [responseMessage, setResponseMessage] = useState(null);

  // 1. Carregar as ordens ao iniciar
  useEffect(() => {
    api.get('/orders/GetOrders')
      .then(response => {
        setOrders(response.data);
      })
      .catch(error => {
        console.error("Erro ao carregar as ordens:", error);
      });
  }, []);

  // 2. Controlar o cronômetro do Tempo de Ciclo
  useEffect(() => {
    if (selectedOrder) {
      setStartTime(Date.now());
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
  }, [selectedOrder]);

  const handleOrderChange = (e) => {
    const orderCode = e.target.value;
    const found = orders.find(o => o.orderCode === orderCode);
    setSelectedOrder(found || null);
    setSelectedMaterial('');
  };

  // Validar se o tempo decorrido atingiu o tempo de ciclo exigido pelo produto
  const requiredCycleTime = selectedOrder?.product?.cycleTime || 0;
  const isTimeValid = elapsedTime >= requiredCycleTime;

  const handleSubmit = async (e) => {
    e.preventDefault();

    const now = new Date();
    const productionDate = now.toISOString().split('T')[0]; // AAAA-MM-DD
    const productionTime = now.toTimeString().split(' ')[0]; // HH:MM:SS

    const payload = {
      email,
      order: selectedOrder.orderCode,
      productionDate,
      productionTime,
      quantity: Number(quantity),
      materialCode: selectedMaterial,
      cycleTime: elapsedTime
    };

    try {
      const response = await api.post('/orders/SetProduction', payload);
      setResponseMessage(response.data);

      // Se sucesso, limpar campos conforme especificação
      if (response.data.status === 200 || response.data.type === 'S') {
        setSelectedOrder(null);
        setQuantity('');
        setSelectedMaterial('');
      }
    } catch (error) {
      console.error("Erro no apontamento:", error);
      setResponseMessage({ status: 500, type: 'E', description: 'Erro de conexão com o servidor.' });
    }
  };

  return (
    <div style={{ padding: '30px', maxWidth: '600px', margin: 'auto', fontFamily: 'Arial' }}>
      <h2>Apontamento de Produção - Sequor</h2>

      <form onSubmit={handleSubmit}>
        <div style={{ marginBottom: '15px' }}>
          <label><strong>1. E-mail:</strong></label><br />
          <input 
            type="email" 
            value={email} 
            onChange={(e) => setEmail(e.target.value)} 
            required 
            style={{ width: '100%', padding: '8px', marginTop: '5px' }}
            placeholder="teste@sequor.com.br"
          />
        </div>

        <div style={{ marginBottom: '15px' }}>
          <label><strong>2. Ordem de Produção:</strong></label><br />
            <select value={selectedOrder?.orderCode || ''} onChange={handleOrderChange} required style={{ width: '100%', padding: '8px', marginTop: '5px' }}>
              <option value="">Selecione uma Ordem</option>
                {Array.isArray(orders) && orders.map(o => (
                  <option key={o.orderCode} value={o.orderCode}>
                      Ordem: {o.orderCode} - Qtd: {o.quantity}
                  </option>
                ))}
            </select>
        </div>

        {selectedOrder && (
          <div style={{ background: '#f4f4f4', padding: '15px', borderRadius: '5px', marginBottom: '15px' }}>
            <h4>Detalhes do Produto</h4>
            <p><strong>Produto:</strong> {selectedOrder.product?.description || selectedOrder.productCode}</p>
            <p><strong>Tempo de Ciclo Exigido:</strong> {requiredCycleTime}s</p>
            {selectedOrder.product?.image && (
              <img 
                src={selectedOrder.product.image} 
                alt="Produto" 
                style={{ maxWidth: '150px', display: 'block', marginTop: '10px', borderRadius: '4px' }} 
              />
            )}
            <p style={{ marginTop: '10px', color: isTimeValid ? 'green' : 'orange' }}>
              ⏱️ Tempo decorrido: <strong>{elapsedTime}s</strong> / {requiredCycleTime}s {isTimeValid ? '✔️ Liberado' : '⏳ Aguardando ciclo...'}
            </p>
          </div>
        )}

        <div style={{ marginBottom: '15px' }}>
          <label><strong>3. Quantidade:</strong></label><br />
          <input 
            type="number" 
            value={quantity} 
            onChange={(e) => setQuantity(e.target.value)} 
            required 
            style={{ width: '100%', padding: '8px', marginTop: '5px' }}
          />
        </div>

        <div style={{ marginBottom: '15px' }}>
          <label><strong>4. Material Utilizado:</strong></label><br />
          <select 
            value={selectedMaterial} 
            onChange={(e) => setSelectedMaterial(e.target.value)} 
            required 
            style={{ width: '100%', padding: '8px', marginTop: '5px' }}
            disabled={!selectedOrder}
          >
            <option value="">Selecione o Material</option>
            {selectedOrder?.product?.productMaterials?.pm?.map(mat => (
              <option key={mat.materialCode} value={mat.materialCode}>
                {mat.materialCode} - {mat.description}
              </option>
            ))}
          </select>
        </div>

        <button 
          type="submit" 
          disabled={!isTimeValid || !selectedOrder}
          style={{ 
            background: isTimeValid ? '#28a745' : '#ccc', 
            color: '#fff', 
            padding: '10px 20px', 
            border: 'none', 
            borderRadius: '4px', 
            cursor: isTimeValid ? 'pointer' : 'not-allowed',
            width: '100%',
            fontSize: '16px'
          }}
        >
          {isTimeValid ? 'Enviar Apontamento' : 'Aguarde o Tempo de Ciclo...'}
        </button>
      </form>

      {responseMessage && (
        <div style={{ 
          marginTop: '20px', 
          padding: '15px', 
          background: responseMessage.type === 'S' ? '#d4edda' : '#f8d3d3',
          color: responseMessage.type === 'S' ? '#155724' : '#721c24',
          borderRadius: '5px'
        }}>
          <p><strong>Status:</strong> {responseMessage.status}</p>
          <p><strong>Tipo:</strong> {responseMessage.type}</p>
          <p><strong>Descrição:</strong> {responseMessage.description}</p>
        </div>
      )}
    </div>
  );
}

export default App;
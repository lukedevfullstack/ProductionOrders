import React from 'react';
import { useProduction } from '../../hooks/useProduction';
import { ProductionForm } from '../../components/ProductionForm';
import { ProductionHistory } from '../../components/ProductionHistory';
import { NotificationModal } from '../../components/NotificationModal';

export default function ProductionPage() {
  const {
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
  } = useProduction();

  return (
    <div className="app-container">
      <div className="main-wrapper">
        
        <h1>Monitoramento e Apontamento - Sequor</h1>

        <div className="card-box">
          <div className="form-group" style={{ marginBottom: 0 }}>
            <label>Identificação do Operador (E-mail):</label>
            <input 
              type="email" 
              value={operatorEmail} 
              onChange={(e) => setOperatorEmail(e.target.value)} 
              placeholder="Digite seu e-mail (obrigatório)"
              required 
            />
          </div>
        </div>

        <div className="nav-buttons">
          <button 
            style={{ backgroundColor: activeTab === 'form' ? '#007bff' : '#2a2a2a', color: '#fff' }}
            onClick={() => setActiveTab('form')}
          >
            Novo Apontamento
          </button>
          <button 
            style={{ backgroundColor: activeTab === 'history' ? '#007bff' : '#2a2a2a', color: '#fff' }}
            onClick={() => setActiveTab('history')}
          >
            Histórico de Produção
          </button>
        </div>

        {activeTab === 'form' ? (
          <ProductionForm 
            productionOrders={productionOrders}
            selectedOrderCode={selectedOrderCode}
            handleOrderCommand={handleOrderChange}
            handleOrderChange={handleOrderChange}
            selectedOrderObj={selectedOrderObj}
            requiredCycleTime={requiredCycleTime}
            elapsedTime={elapsedTime}
            isCycleTimeValid={isCycleTimeValid}
            quantity={quantity}
            setQuantity={setQuantity}
            selectedMaterial={selectedMaterial}
            setSelectedMaterial={setSelectedMaterial}
            isFormValid={isFormValid}
            operatorEmail={operatorEmail}
            submitProduction={submitProduction}
          />
        ) : (
          <ProductionHistory 
            history={history}
            operatorEmail={operatorEmail}
          />
        )}

        {/* Modal Customizado */}
        <NotificationModal 
          isOpen={modalState.isOpen}
          type={modalState.type}
          message={modalState.message}
          onClose={closeModal}
        />

      </div>
    </div>
  );
}
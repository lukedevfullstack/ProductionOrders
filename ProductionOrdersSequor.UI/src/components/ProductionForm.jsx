import React from 'react';

export function ProductionForm({
  productionOrders,
  selectedOrderCode,
  handleOrderChange,
  selectedOrderObj,
  requiredCycleTime,
  elapsedTime,
  isCycleTimeValid,
  quantity,
  setQuantity,
  selectedMaterial,
  setSelectedMaterial,
  isFormValid,
  operatorEmail,
  submitProduction
}) {
  return (
    <form className="card-box" onSubmit={submitProduction}>
      <div className="form-group">
        <label>Ordem de Produção:</label>
        <select value={selectedOrderCode} onChange={handleOrderChange} required>
          <option value="">Selecione uma Ordem</option>
          {Array.isArray(productionOrders) && productionOrders.map((order, index) => (
            <option key={index} value={order.order}>
              Ordem: {order.order} - Qtd: {order.quantity} ({order.productDescription})
            </option>
          ))}
        </select>
      </div>

      {selectedOrderObj && (
        <div style={{ background: '#222', padding: '12px', borderRadius: '4px', marginBottom: '15px', fontSize: '0.9rem', color: '#ccc' }}>
          <p><strong>Produto:</strong> {selectedOrderObj.productDescription} ({selectedOrderObj.productCode})</p>
          <p><strong>Tempo de Ciclo Exigido:</strong> {requiredCycleTime}s</p>
          
          <p style={{ marginTop: '8px', color: isCycleTimeValid ? '#28a745' : '#ffc107' }}>
            ⏱️ Tempo decorrido: <strong>{elapsedTime}s</strong> / {requiredCycleTime}s {isCycleTimeValid ? '✔️ Liberado' : '⏳ Aguardando ciclo...'}
          </p>

          {selectedOrderObj.image && (
            <div style={{ marginTop: '10px' }}>
              <img 
                src={selectedOrderObj.image} 
                alt="Produto" 
                style={{ maxWidth: '120px', borderRadius: '4px', border: '1px solid #444', display: 'block' }} 
              />
            </div>
          )}
        </div>
      )}

      <div className="form-group">
        <label>Quantidade:</label>
        <input 
          type="number" 
          placeholder="Digite a quantidade" 
          value={quantity}
          onChange={(e) => setQuantity(e.target.value)}
          required 
        />
      </div>

      <div className="form-group">
        <label>Material Utilizado:</label>
        <select 
          value={selectedMaterial}
          onChange={(e) => setSelectedMaterial(e.target.value)}
          required
          disabled={!selectedOrderObj}
          style={{ width: '100%', padding: '8px', background: '#333', color: '#fff', border: '1px solid #555', borderRadius: '4px' }}
        >
          <option value="">Selecione o Material</option>
          {selectedOrderObj?.materials?.map((mat, idx) => (
            <option key={idx} value={mat.materialCode}>
              {mat.materialCode} - {mat.materialDescription}
            </option>
          ))}
        </select>
      </div>

      <button 
        type="submit" 
        className="btn-submit"
        disabled={!isFormValid}
        style={{ 
          backgroundColor: isFormValid ? '#28a745' : '#555', 
          cursor: isFormValid ? 'pointer' : 'not-allowed' 
        }}
      >
        {!operatorEmail.trim() 
          ? 'Informe o E-mail do Operador' 
          : (!isCycleTimeValid ? 'Aguarde o Tempo de Ciclo...' : 'Enviar Apontamento')}
      </button>
    </form>
  );
}
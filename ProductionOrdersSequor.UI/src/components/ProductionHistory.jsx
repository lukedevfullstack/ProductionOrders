import React from 'react';

export function ProductionHistory({ history, operatorEmail }) {
  return (
    <div className="card-box">
      <h3>Histórico de Apontamentos</h3>
      <p style={{ fontSize: '0.85rem', color: '#aaa', marginBottom: '15px' }}>
        A listar histórico para o e-mail: {operatorEmail || '(informe um e-mail)'}
      </p>
      
      <div className="table-responsive">
        <table>
          <thead>
            <tr>
              <th>Ordem</th>
              <th>Qtd</th>
              <th>Material</th>
              <th>Data/Hora</th>
              <th>E-mail</th>
            </tr>
          </thead>
          <tbody>
            {history.length === 0 ? (
              <tr>
                <td colSpan="5" style={{ textAlign: 'center', color: '#888', padding: '20px' }}>
                  Nenhum registro encontrado.
                </td>
              </tr>
            ) : (
              history.map((item, index) => (
                <tr key={index}>
                  <td>{item.order || item.orderId}</td>
                  <td>{item.quantity}</td>
                  <td>{item.materialDescription}</td>
                  <td>{item.date || item.createdAt || `${item.productionDate || ''} ${item.productionTime || ''}`}</td>
                  <td>{item.email || operatorEmail}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
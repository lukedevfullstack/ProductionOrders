import React from 'react';

export function NotificationModal({ isOpen, type, message, onClose }) {
  if (!isOpen) return null;

  const isError = type === 'error';

  return (
    <div style={{
      position: 'fixed',
      top: 0,
      left: 0,
      width: '100%',
      height: '100%',
      backgroundColor: 'rgba(0, 0, 0, 0.7)',
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      zIndex: 1000,
      animation: 'fadeIn 0.2s ease-in-out'
    }}>
      <div style={{
        backgroundColor: '#1e1e1e',
        border: `1px solid ${isError ? '#dc3545' : '#28a745'}`,
        padding: '24px',
        borderRadius: '8px',
        width: '100%',
        maxWidth: '400px',
        boxShadow: '0 4px 20px rgba(0,0,0,0.5)',
        textAlign: 'center',
        color: '#fff'
      }}>
        <div style={{
          fontSize: '2.5rem',
          marginBottom: '10px'
        }}>
          {isError ? '⚠️' : '✅'}
        </div>
        
        <h3 style={{
          margin: '0 0 10px 0',
          color: isError ? '#dc3545' : '#28a745'
        }}>
          {isError ? 'Atenção' : 'Sucesso'}
        </h3>
        
        <p style={{
          fontSize: '0.95rem',
          color: '#ccc',
          marginBottom: '20px',
          lineHeight: '1.4'
        }}>
          {message}
        </p>

        <button 
          onClick={onClose}
          style={{
            backgroundColor: isError ? '#dc3545' : '#28a745',
            color: '#fff',
            border: 'none',
            padding: '10px 20px',
            borderRadius: '4px',
            cursor: 'pointer',
            fontWeight: 'bold',
            width: '100%',
            transition: 'opacity 0.2s'
          }}
          onMouseOver={(e) => e.target.style.opacity = '0.9'}
          onMouseOut={(e) => e.target.style.opacity = '1'}
        >
          OK
        </button>
      </div>
    </div>
  );
}
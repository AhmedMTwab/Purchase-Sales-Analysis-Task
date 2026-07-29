import React from 'react';
import { FiAlertCircle, FiCheckCircle } from 'react-icons/fi';

const StatCard = ({ title, icon, children, isLoading, message, onAction, actionLabel, customContent }) => (
  <div className="glass-card" style={{ height: '100%' }}>
    <div style={{ padding: '1.5rem' }}>
      {/* Header */}
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '1.25rem' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
          <div style={{
            width: '38px', height: '38px',
            background: 'rgba(99,102,241,0.12)',
            border: '1px solid rgba(99,102,241,0.2)',
            borderRadius: '10px',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: '#818cf8',
            flexShrink: 0,
          }}>
            {icon}
          </div>
          <h3 style={{ fontWeight: 700, fontSize: '0.95rem', color: '#f1f5f9', margin: 0 }}>{title}</h3>
        </div>
        {onAction && (
          <button
            onClick={onAction}
            disabled={isLoading}
            className="btn-secondary"
            style={{ padding: '0.4rem 0.875rem', fontSize: '0.8rem' }}
          >
            {isLoading ? (
              <>
                <div className="spinner" style={{ width: 13, height: 13 }} />
                Loading...
              </>
            ) : actionLabel}
          </button>
        )}
      </div>

      {/* Message */}
      {message && (
        <div className={message.includes('Error') || message.includes('No ') ? 'alert-error' : 'alert-success'}
          style={{ marginBottom: '1rem' }}>
          {message.includes('Error') || message.includes('No ')
            ? <FiAlertCircle size={15} style={{ flexShrink: 0 }} />
            : <FiCheckCircle size={15} style={{ flexShrink: 0 }} />
          }
          <span>{message}</span>
        </div>
      )}

      {customContent || children}
    </div>
  </div>
);

export default StatCard;

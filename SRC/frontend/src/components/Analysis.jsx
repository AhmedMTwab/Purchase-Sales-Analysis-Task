import React from 'react';
import TopSalesCard from './TopSalesCard';
import DeadstockCard from './DeadstockCard';
import ProductProfitCard from './ProductProfitCard';
import { FiBarChart2, FiInfo } from 'react-icons/fi';

function Analysis() {
  return (
    <div>
      {/* Page header */}
      <div className="page-header">
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem', marginBottom: '0.5rem' }}>
          <div style={{
            width: '38px', height: '38px',
            background: 'rgba(99,102,241,0.12)',
            border: '1px solid rgba(99,102,241,0.2)',
            borderRadius: '10px',
            display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: '#818cf8',
          }}>
            <FiBarChart2 size={18} />
          </div>
          <div>
            <h1 style={{ margin: 0, fontSize: '1.5rem', fontWeight: 800, color: '#f1f5f9', letterSpacing: '-0.02em' }}>
              Market Analysis
            </h1>
            <p style={{ margin: 0, fontSize: '0.85rem', color: '#64748b' }}>
              Insights into sales performance and inventory health
            </p>
          </div>
        </div>
      </div>

      {/* Cards grid */}
      <div style={{
        display: 'grid',
        gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
        gap: '1.25rem',
        marginBottom: '1.5rem',
      }}>
        <TopSalesCard />
        <DeadstockCard />
        <ProductProfitCard />
      </div>

      {/* Info banner */}
      <div style={{
        display: 'flex',
        alignItems: 'flex-start',
        gap: '0.875rem',
        padding: '1rem 1.25rem',
        background: 'rgba(99,102,241,0.06)',
        border: '1px solid rgba(99,102,241,0.15)',
        borderRadius: '12px',
      }}>
        <FiInfo size={16} style={{ color: '#6366f1', flexShrink: 0, marginTop: '0.1rem' }} />
        <p style={{ margin: 0, fontSize: '0.83rem', color: '#64748b', lineHeight: 1.6 }}>
          <span style={{ fontWeight: 600, color: '#818cf8' }}>Pro Tip:</span>{' '}
          Use these analytics to identify your best-selling products, manage inventory levels,
          and optimize your pricing strategy for maximum profitability.
        </p>
      </div>
    </div>
  );
}

export default Analysis;
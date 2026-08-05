import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import './index.css'
import Layout from './components/Layout'
import AnalysisPage from './pages/AnalysisPage'
import ProductPage from './pages/ProductPage'
import UploadPage from './pages/UploadPage'

const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    children: [
      {
        path: '/',
        element: <AnalysisPage />
      },
      {
        path: '/products',
        element: <ProductPage />
      },
      {
        path: '/upload',
        element: <UploadPage />
      },
      {
        path: '/analysis',
        element: <AnalysisPage />
      }
    ]
  }
])

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
)

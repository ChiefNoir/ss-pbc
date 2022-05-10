import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { Navigation } from './ui/navigation'
import reportWebVitals from './reportWebVitals';
import Header from './features/ui/header/header'
import Footer from './features/ui/footer/footer'
import { BrowserRouter } from "react-router-dom";

import AppRouter from './AppRouter';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <React.StrictMode>
    <Header/>

    <BrowserRouter>
      <AppRouter />
    </BrowserRouter>

    <Footer/>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

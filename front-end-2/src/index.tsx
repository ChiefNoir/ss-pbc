import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { Introduction } from './features/introduction';
import { NotFound } from './features/not-found';
import { Navigation } from './features/navigation'
import reportWebVitals from './reportWebVitals';
import Header from './features/ui/header/header'

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
root.render(
  <React.StrictMode>
    <Header/>
    <Navigation />
    <Introduction />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();

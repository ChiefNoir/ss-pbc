import React from 'react';
import logo from '../../../src/logo.svg';
import './introduction.css';
import Navigation from '../ui/navigation/navigation';

function Introduction() {
  let something = "Somthinw";
  return (
    <div className="App">
      <Navigation />
      
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          {process.env.REACT_APP_API}
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn {something}
        </a>
      </header>
    </div>
  );
}

export default Introduction;

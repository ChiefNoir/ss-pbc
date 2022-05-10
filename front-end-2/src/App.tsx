import * as React from "react";
import { BrowserRouter as Router,
  Route,
  Routes,
  Link,
  useLocation } from "react-router-dom";
import { Introduction } from './features/introduction';
import { NotFound } from './features/not-found';

function App() {
  return (
    <Routes>
        <Route path="/" element={<Introduction />} />
        <Route path="*" element={<NotFound />} />
      </Routes>
  );
}

export default App;
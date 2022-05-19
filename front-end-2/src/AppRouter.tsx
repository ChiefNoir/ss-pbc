import React from 'react';
import {Route, Routes} from 'react-router-dom';
import IntroductionPage  from './pages/introduction/introductionPage';
import { NotFound } from './pages/not-found';

const AppRouter = () => (
    <Routes>
        <Route path='/' element={<IntroductionPage />} />
        <Route path="*" element={<NotFound />} />
    </Routes>
);

export default AppRouter;


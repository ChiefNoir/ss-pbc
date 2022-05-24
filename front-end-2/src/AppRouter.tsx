import React from 'react';
import {Route, Routes} from 'react-router-dom';
import { NotFoundPage, IntroductionPage } from './pages/_index'

const AppRouter = () => (
    <Routes>
        <Route path='/' element={<IntroductionPage />} />
        <Route path="*" element={<NotFoundPage />} />
    </Routes>
);

export default AppRouter;


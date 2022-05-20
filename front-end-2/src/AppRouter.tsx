import React from 'react';
import {Route, Routes} from 'react-router-dom';
import { NotFound, IntroductionPage } from './pages/_index'

const AppRouter = () => (
    <Routes>
        <Route path='/' element={<IntroductionPage />} />
        <Route path="*" element={<NotFound />} />
    </Routes>
);

export default AppRouter;


import React from 'react';
import {Route, Routes, Navigate } from 'react-router-dom';
import { NotFoundPage, IntroductionPage, ProjectsPage } from './pages/_index'

const AppRouter = () => (
    <Routes>
        <Route path='/' element={<IntroductionPage />} />

        <Route path="projects" element={<ProjectsPage />}>
          <Route path=":categoryCode" element={<ProjectsPage />} />
          <Route path=":categoryCode/:page" element={<ProjectsPage />} />
        </Route>

        <Route path="*" element={<NotFoundPage />} />
    </Routes>
);

export default AppRouter;

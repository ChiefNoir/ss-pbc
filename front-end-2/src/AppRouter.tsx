import {Route, Routes } from 'react-router-dom';
import { NotFoundPage, IntroductionPage, ShowcasePage, ProjectPage } from './pages'

const AppRouter = () => (
    <Routes>
        <Route path='/' element={<IntroductionPage />} />

        <Route path="projects" element={<ShowcasePage />}>
          <Route path="?category=:categoryCode" element={<ShowcasePage />} />
          <Route path="?category=:categoryCode&page=:page" element={<ShowcasePage />} />
        </Route>

        <Route path='projects/:projectCode' element={<ProjectPage />} />

        <Route path="*" element={<NotFoundPage />} />
    </Routes>
);

export default AppRouter;

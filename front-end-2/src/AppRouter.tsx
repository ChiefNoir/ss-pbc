import {Route, Routes } from 'react-router-dom';
import { NotFoundPage, IntroductionPage, ShowcasePage, ProjectPage } from './pages'

const AppRouter = () => (
    <Routes>
        <Route path='/' element={<IntroductionPage />} />

        <Route path="projects" element={<ShowcasePage />}>
          <Route path=":categoryCode" element={<ShowcasePage />} />
          <Route path=":categoryCode/:page" element={<ShowcasePage />} />
        </Route>

        <Route path='project/:projectCode' element={<ProjectPage />} />

        <Route path="*" element={<NotFoundPage />} />
    </Routes>
);

export default AppRouter;

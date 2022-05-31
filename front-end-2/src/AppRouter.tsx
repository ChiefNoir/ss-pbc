import { Route, Routes } from "react-router-dom";
import { ErrorNotFoundPage, IntroductionPage, ShowcasePage, ProjectPage, CoreLayout } from "./pages";

const AppRouter = () => (
    <Routes>
        
        <Route path="projects/:projectCode" element={<ProjectPage />} />

        <Route path="/" element={<CoreLayout />} >
        <Route path="*" element={<ErrorNotFoundPage />} />


          <Route index element={<IntroductionPage />} />

          <Route path="projects" element={<ShowcasePage />}>
            <Route path="?category=:categoryCode" element={<ShowcasePage />} />
            <Route path="?category=:categoryCode&page=:page" element={<ShowcasePage />} />
          </Route>
        </Route>
    </Routes>
);

export { AppRouter };

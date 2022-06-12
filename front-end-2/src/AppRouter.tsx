import { Route, Routes } from "react-router-dom";
import * as Pages from "./pages";
import * as PagesAdmin from "./pages-admin";

const AppRouter = () => (
  <Routes>
    <Route path="/" element={<Pages.CoreLayout />} >
      <Route path="*" element={<Pages.ErrorNotFoundPage />} />

      <Route index element={<Pages.IntroductionPage />} />

      <Route path="projects/:projectCode" element={<Pages.ProjectPage />} />

      <Route path="projects" element={<Pages.ShowcasePage />}>
        <Route path="?category=:categoryCode" element={<Pages.ShowcasePage />} />
        <Route path="?category=:categoryCode&page=:page" element={<Pages.ShowcasePage />} />
      </Route>

      <Route path="login" element={<Pages.LoginPage />} />

      <Route path="admin/intro" element={<PagesAdmin.AdminIntroductionPage />} />
      <Route path="admin/accounts" element={<PagesAdmin.AdminAccountsPage />} />
      <Route path="admin/projects" element={<PagesAdmin.AdminProjectsPage />} />
      <Route path="admin/categories" element={<PagesAdmin.AdminCategoriesPage />} />
    </Route>
  </Routes>
);

export { AppRouter };

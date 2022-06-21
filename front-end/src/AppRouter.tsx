import React, { lazy } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import * as Pages from "./pages";
import { Loader } from "./ui";

const AdminLayout = lazy(() =>
  import("./pages-admin")
    .then(({ AdminLayout }) => ({ default: AdminLayout }))
);
const AdminAccountsPage = lazy(() =>
  import("./pages-admin")
    .then(({ AdminAccountsPage }) => ({ default: AdminAccountsPage }))
);
const AdminCategoriesPage = lazy(() =>
  import("./pages-admin")
    .then(({ AdminCategoriesPage }) => ({ default: AdminCategoriesPage }))
);
const AdminIntroductionPage = lazy(() =>
  import("./pages-admin")
    .then(({ AdminIntroductionPage }) => ({ default: AdminIntroductionPage }))
);
const AdminProjectsPage = lazy(() =>
  import("./pages-admin")
    .then(({ AdminProjectsPage }) => ({ default: AdminProjectsPage }))
);

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
    </Route>

    <Route path="/admin" element={
                            <React.Suspense fallback={<Loader/>}>
                              <AdminLayout />
                            </React.Suspense>}>

      <Route path="" element={<Navigate to="intro" replace />} />

      <Route path="intro" element={
                            <React.Suspense fallback={<Loader/>}>
                              <AdminIntroductionPage />
                            </React.Suspense>} />
      <Route path="accounts" element={
                            <React.Suspense fallback={<Loader/>}>
                              <AdminAccountsPage />
                            </React.Suspense>} />
      <Route path="projects" element={
                            <React.Suspense fallback={<Loader/>}>
                              <AdminProjectsPage />
                            </React.Suspense>} />
      <Route path="categories" element={
                            <React.Suspense fallback={<Loader/>}>
                              <AdminCategoriesPage />
                            </React.Suspense>} />
    </Route>
  </Routes>
);

export { AppRouter };

import React from 'react';
import {Route, Routes} from 'react-router-dom';
// import Page404 from "./pages/404Page";
// import HomePage from "./pages/HomePage";
// import SubPage from './pages/SubPage';
import { Introduction } from './features/introduction';
import { NotFound } from './pages/not-found';
import { Loader } from './ui/loader';
import loadable from 'react-loadable';

// const LoadableComponent = loadable({
//     loader: () => import('./pages/not-found/not-found'),
//         loading: Loader
//   });
const AppRouter = () => (

    <Routes>
        <Route path='/' element={<Introduction />} />

        {/* <Route path='/something' element={<LoadableComponent/>} /> */}

        <Route path="*" element={<NotFound />} />
    </Routes>

);

export default AppRouter;
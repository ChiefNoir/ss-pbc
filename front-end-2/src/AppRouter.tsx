import React from 'react';
import {Route, Routes} from 'react-router-dom';
// import Page404 from "./pages/404Page";
// import HomePage from "./pages/HomePage";
// import SubPage from './pages/SubPage';
import { Introduction } from './pages/introduction';
import { NotFound } from './pages/not-found';
import { Loader } from './ui/loader';
import loadable from 'react-loadable';

const LoadingPage = () => (
    <div>
        <p>
            Page is loading
        </p>
    </div>
);

const AsyncPages = {
    myAsyncSubPage: loadable({
        loader: () => import('./pages/introduction/introduction'),
        loading: LoadingPage
    })
};
const AppRouter = () => (

    <Routes>
        <Route path='/' element={<Introduction />} />

        {/* <Route path='/something' element={<LoadableComponent/>} /> */}

        <Route path="*" element={<NotFound />} />
    </Routes>

);

export default AppRouter;


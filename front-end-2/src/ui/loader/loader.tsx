import React from 'react';
import logo from '../../../src/logo.svg';
import './loader.scss';
import { useTranslation } from "react-i18next";
import "../../locales/i18n";

function Loader() {
  const { t } = useTranslation();
  return (
    <div>loading</div>
  );
}

export default Loader;

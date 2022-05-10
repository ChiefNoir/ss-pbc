import React from 'react';
import logo from '../../../src/logo.svg';
import './navigation.scss';
import { useTranslation } from "react-i18next";
import "../../locales/i18n";

function Navigation() {
  const { t } = useTranslation();
  return (
    <div>{t("navigate")}</div>
  );
}

export default Navigation;

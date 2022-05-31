import { Outlet } from "react-router-dom";
import "./core-layout.scss";

function CoreLayout() {
    return (
      <div className="core-layout-container">
        <Outlet />
      </div>
    );
  }

  export { CoreLayout };
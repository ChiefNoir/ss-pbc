import { Outlet } from "react-router-dom";
import { NavigationAdmin } from "../../features";
import "./admin-layout.scss";

function AdminLayout() {
  return (
  <div className="admin-layout-container">
    <NavigationAdmin />
    <Outlet />
  </div>
  );
}

export { AdminLayout };

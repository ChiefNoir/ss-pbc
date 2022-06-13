import { ReactElement } from "react";
import { Navigate, Outlet, useLocation } from "react-router-dom";
import { NavigationAdmin } from "../../features";
import { store } from "../../storage";
import "./admin-layout.scss";

function RequireAuth({ children }: { children: ReactElement }) {
  const auth = store.getState().identity.value;
  const location = useLocation();

  if (!auth) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
}

function AdminLayout() {
  return (
    <RequireAuth>
      <div className="admin-layout-container">
        <NavigationAdmin />
        <Outlet />
      </div>
  </RequireAuth>
  );
}

export { AdminLayout };

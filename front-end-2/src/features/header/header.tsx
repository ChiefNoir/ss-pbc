import "./header.scss";
import { Link } from "react-router-dom";

function Header() {
  return (
  <header>
    <Link className="full-size" to="/"> {process.env.REACT_APP_NAME} </Link>
    <Link className="small-size" to="/"> {process.env.REACT_APP_NAME_SHORT} </Link>
  </header>
  );
}

export { Header };

import './header.scss';
import { Link } from 'react-router-dom';

function Header() {
  return (
    <div className="header">
      <Link to='/'>{process.env.REACT_APP_API_AUTH_ENDPOINT}</Link>
    </div>
  );
}

export default Header;

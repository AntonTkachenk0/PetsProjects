import { Link } from "react-router-dom";

export default function Navbar(){
    return (
       <>
      <nav className="nav-bar">
      <ul style={{ display: 'flex', listStyle: 'none', gap: '20px' }}>
        <li>
          <Link to="/">Главная</Link>
        </li>
        <li>
          <Link to="/pet">Создание питомца</Link>
        </li>
        <li>
          <Link to="/user">Регистрация</Link>
        </li>
         <li>
          <Link to="/service">Услуги</Link>
        </li>
      </ul>
    </nav>
       </>
    );
}
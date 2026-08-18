import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { fetchUser, fetchUserPhotoBlob } from "../services/users";

export default function Navbar({ photoUrl }) {




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
          <li>
            <Link to="/serviceJournal">Записаться на услугу</Link>
          </li>
          <li style={{ marginLeft: 'auto', display: 'flex', alignItems: 'center', gap: '10px' }}>
            <Link to="/profile">
              <img src={photoUrl}
                alt="Avatar"
                style={{
                  width: '70px',
                  height: '70px',
                  borderRadius: '50%',
                  objectFit: 'cover',
                }} />
            </Link>
          </li>
        </ul>

      </nav>
    </>
  );
}
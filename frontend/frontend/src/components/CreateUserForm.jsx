import { useState } from "react";
import { fetchUsers } from "../services/users";
import { useEffect } from 'react';

   
   export default function createUserForm( {onCreate}) {
  
    const [user, setUser] = useState(null);

    function handleSubmit(e) {
        e.preventDefault();
        setUser(null);
       onCreate(user);
    }
    
    return (
    <form className="user-form" onSubmit={handleSubmit}>
      <h3>Регистрация пользователя</h3>
      <input 
      className="user-input" 
      placeholder='Введите логин' 
      onChange={(e) => setUser({...user, login: e.target.value})} />
      <input 
      className="user-input" 
       placeholder='Введите имя'
        onChange={(e) => setUser({...user, name: e.target.value})} />
      <input 
      className="user-input" 
       placeholder='Введите пароль' 
       onChange={(e) => setUser({...user, passwordHached: e.target.value})}/>
      <input 
      className="user-input" 
       placeholder='Введите email'
       onChange={(e) => setUser({...user, email: e.target.value})} />
      <button 
      className="user-button"
       type="submit">Отправить</button>
    </form>
    );
   }
  
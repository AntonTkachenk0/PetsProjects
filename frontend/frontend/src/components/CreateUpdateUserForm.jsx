import { useState } from "react";

export default function CreateUpdateUserForm({onCreate, user}) {

      const [userUpdate, setUserUpdate] = useState([]);


      function onSubmit(e){
        e.preventDefault();
        onCreate(userUpdate);
        setUserUpdate(null);
        
      }
    return (
        <form onSubmit={onSubmit}>
            <p>Имя</p>
            <input
                className="name-input"
                placeholder={user?.name || 'Имя не указано'}
                onChange={(e) => setUserUpdate({ ...userUpdate, name: e.target.value })} />
            <p>Логин</p>
            <input
                className="name-input"
                placeholder={user?.login || 'Логин не указан'}
                onChange={(e) => setUserUpdate({ ...userUpdate, login: e.target.value })} />
            <p>Почта</p>
            <input
                className="name-input"
                placeholder={user?.email || 'Почта не указана'}
                onChange={(e) => setUserUpdate({ ...userUpdate, email: e.target.value })} />

            <button
                className="pet-button"
                type="submit"
                
                >Отправить</button>
        </form>
    );
}
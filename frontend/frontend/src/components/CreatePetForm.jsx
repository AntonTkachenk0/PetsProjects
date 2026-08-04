import { useEffect, useState } from "react";
import { fetchUsers } from "../services/users";

             export default function createPetForm( {onCreate}) {

                useEffect(() => {
                    const fetchData = async () => {
                    let data = await fetchUsers();
            
                    console.log(data);
                    setUsers(data);
                    };
            
                    fetchData();
                }, []);

                const [users, setUsers] = useState([]);
                const [pet, setPet] = useState(null);
            
                function handleSubmit(e) {
                    e.preventDefault();
                    console.log(pet)
                    onCreate(pet);
                    setPet(null);
                }
                
                return (
                <form className="pet-form" 
                onSubmit={handleSubmit}>
                  <h3>Регистрация питомца</h3>
                  <input 
                   className="pet-input"
                  placeholder='Введите имя' 
                  onChange={(e) => setPet({...pet, name: e.target.value})} />
                  <a>Выберите владельца</a>
                    <select 
                    className="pet-input"
                     name="user" 
                     id="user"
                    onChange={(e) => setPet({...pet, id_User: (e.target.value)})}
                     >
                         <option value="">-- Выберите пользователя --</option>
                        {users.map(user => (
                        <option key={user.id} value={user.id}>
                            {user.name}
                        </option>
                        ))}
                    </select>
                  <input 
                  className="pet-input"
                  placeholder='Введите пол'
                   onChange={(e) => setPet({...pet, gender: e.target.value})}/>
                  <input 
                   className="pet-input"
                  placeholder='Введите породу'
                  onChange={(e) => setPet({...pet, breed: e.target.value})} />
                  <button 
                   className="pet-button"
                  type="submit">Отправить</button>
                </form>
                );
               }
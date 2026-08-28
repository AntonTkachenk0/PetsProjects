import axios from "axios";
import { useEffect, useState } from "react";
import { fetchUserPhotoBlob, getHistory, getStats } from "../services/users";
import { fetchPets } from "../services/pets";
import CreateCard from './CreateCard';
import CreateChangePhotoPet from "./CreateChangePhotoPet";
import moment from 'moment';
export default function CreateProfile({ user, photoUrl }) {

    const [pets, setPets] = useState([]);

    const userId = 'a4e151dd-9f79-4033-b61b-8a62d9c9a27c';

    const [showPets, setShowPets] = useState(false);
    const [showStats, setShowStats] = useState(false);
    const [showTable, setShowTable] = useState(false);
    const [stats, setStats] = useState(null);
    const [history, setHistory] = useState([]);
    useEffect(() => {
        const loadData = async () => {
            var dataPets = await fetchPets();
            var dataStats = await getStats(userId);
            var dataHistory = await getHistory(userId);

            setStats(dataStats);
            setPets(dataPets);
            setHistory(dataHistory);
            console.log(dataHistory);
        };


        loadData();

    }, [userId])

    const handleClickPets = () => {
        setShowPets(!showPets);
        setShowStats(false);
        setShowTable(false);
    };

    const handleClickStats = () => {
        setShowPets(false);
        setShowTable(false);
        setShowStats(!showStats);
    };
      const handleClickTable = () => {
        setShowPets(false);
        setShowStats(false);
        setShowTable(!showTable);
    };


    return (
        
        <div>
            <CreateChangePhotoPet entityId={userId} role={"user"} />
            <div className='user-profile-photo'>
                <img
                    src={photoUrl}
                />
            </div>

            <div className="profile-container">
                <p onClick={handleClickPets}>Питомцы</p>
                <p onClick={handleClickStats}>Статистика</p>
            </div>
            <div className="profile-blocks">
                <div className="profile-block-left">
                    <form>
                        <p>Имя</p>
                        <input
                            className="name-input"
                            placeholder={user?.name || 'Имя не указано'} />
                        <p>Логин</p>
                        <input
                            className="name-input"
                            placeholder={user?.login || 'Логин не указан'} />
                        <p>Почта</p>
                        <input
                            className="name-input"
                            placeholder={user?.email || 'Почта не указана'} />


                    </form>
                </div>
                <div className="profile-block-right">
                    {showPets && (
                        <div className="profile-container-pets">
                            <p>Мои питомцы:</p>
                            {pets.map((pet) => (
                                <CreateCard
                                    key={pet.id}
                                    pet={pet}
                                />
                            ))}
                        </div>
                    )}
                    {showStats && (
                        <div >
                            <p>Общая потраченная сумма: {stats !== null ? stats : 'Нет данных'}</p>
                        </div>
                    )}
                </div>
                <div >
                    <table>
                        <thead>
                            <tr>
                                <th>Имя</th>
                                <th>Услуга</th>
                                <th>Дата</th>
                                <th>Стоимость</th>
                            </tr>
                        </thead>
                        <tbody>
                            {history.map(item => (
                                <tr key={`${item.petId}-${item.visitDate}`}>
                                   
                                    <td>{item.petName}</td>
                                    <td>{item.serviceTitle}</td>
                                    <td>{moment(item.visitDate).format("DD.MM.YYYY HH:mm")}</td>
                                    <td>{item.serviceCost}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>



        </div>
    );


}
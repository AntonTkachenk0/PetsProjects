import axios from "axios";
import { useEffect, useState } from "react";
import { fetchUserPhotoBlob, getHistory, getStats, updateUserInfo } from "../services/users";
import { fetchPets } from "../services/pets";
import CreateCard from './CreateCard';
import CreateChangePhotoPet from "./CreateChangePhotoPet";
import moment from 'moment';
import CreateUpdateUserForm from "./CreateUpdateUserForm";
export default function CreateProfile({ user, photoUrl }) {

    const [pets, setPets] = useState([]);

    const userId = 'e1b2468b-154d-49a3-bb72-28f227e148f4';

    const [showPets, setShowPets] = useState(true);
    const [showStats, setShowStats] = useState(false);
    const [stats, setStats] = useState(null);
    const [history, setHistory] = useState([]);
    useEffect(() => {
        const loadData = async () => {
            var dataPets = await fetchPets();
            var dataStats = await getStats(userId);
            console.log(userId);
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
    };

    const handleClickStats = () => {
        setShowPets(false);
        setShowStats(!showStats);
    };


    const handleUpdateUserInfo = (userInfo) => {
        const dataToSend = {
            ...userInfo,
            id: userId,
        };
        updateUserInfo(dataToSend);
    }


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
                    <CreateUpdateUserForm onCreate={handleUpdateUserInfo} user={user} />
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
                    )}
                </div>
            </div>



        </div>
    );


}
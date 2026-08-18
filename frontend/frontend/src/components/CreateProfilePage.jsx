import axios from "axios";
import { useEffect, useState } from "react";
import { fetchUserPhotoBlob } from "../services/users";
import { fetchPets } from "../services/pets";
import CreateCard from './CreateCard';
import CreateChangePhotoPet from "./CreateChangePhotoPet";
export default function CreateProfile({ user, photoUrl }) {

    const [pets, setPets] = useState([]);


    const userId = 'a4e151dd-9f79-4033-b61b-8a62d9c9a27c';


    useEffect(() => {
        const loadData = async () => {
            var data = await fetchPets();
            console.log(data);
            setPets(data);
        };


        loadData();

    }, [userId])




    return (
        <div>
            <CreateChangePhotoPet entityId={userId} role={"user"} />
            <div className='user-profile-photo'>
                <img
                    src={photoUrl}
                />
            </div>

            <div className="profile-container">
                <p>sad</p>

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
                    <p>Мои питомцы:</p>
                    <div className="profile-container-pets">
                        {pets.map((pet) => (
                            <CreateCard
                                key={pet.id}
                                pet={pet}
                            />
                        ))}
                    </div>
                </div>
            </div>



        </div>
    );


}
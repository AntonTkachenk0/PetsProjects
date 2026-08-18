import { useEffect, useState } from 'react';
import catImage from '../assets/cat.jpg';
import { fetchPhotoBlob } from '../services/pets';
import ModalPets from './ModalPets';
import CreateChangePhotoPet from './CreateChangePhotoPet';

export default function CreateCard({ pet }) {

    const [photoUrl, setPhotoUrl] = useState(null);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const openModal = () => setIsModalOpen(true);
    const closeModal = () => setIsModalOpen(false);

    const onSubmit = () => {
        console.log(pet.name);
        openModal();
    }


    useEffect(() => {

        const loadPhoto = async () => {

            const blob = await fetchPhotoBlob(pet.photo_Id);

            const url = URL.createObjectURL(blob);

            setPhotoUrl(url);

        }

        loadPhoto();

    }, [pet.petId])

    return (
        <>
            <div className='pet-card'>
                <div className='pet-avatar'>
                    <img src={photoUrl} />
                </div>

                <div>
                    <h3>{pet.name}</h3>
                    <p>Пол: {pet.gender}</p>
                    <p>Порода: {pet.breed}</p>

                    <button 
                    className='pet-button-card'
                    onClick={openModal}>Открыть</button>

                    <ModalPets
                        isOpen={isModalOpen}
                        onClose={closeModal}
                        pet={pet}
                        photoUrl={photoUrl}
                    />
                </div>

            </div>
        </>
    );

}
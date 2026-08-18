import { useState } from 'react';
import Modal from 'react-modal';
import CreateChangePhotoPet from './CreateChangePhotoPet';

Modal.setAppElement('#root');
export default function ModalPets({ isOpen, onClose, pet, photoUrl }) {

    

    return (
        <Modal isOpen={isOpen} onRequestClose={onClose} contentLabel='тест'>

            <div className='pet-card'>
                <div className='pet-avatar'>
                    <img src={photoUrl} />
                </div>
                <CreateChangePhotoPet entityId={pet.id} role={"pet"}/>
                <form className="pet-form">
                    <p>Кличка: {pet.name}</p>
                    <p>Пол: {pet.gender}</p>
                    <p>Порода {pet.breed}</p>


                    <input
                        type="text"
                        placeholder="Введите пол"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                </form>

                <button type="button" onClick={onClose} className="btn-secondary">Отмена</button>
            </div>

        </Modal>

    );

}
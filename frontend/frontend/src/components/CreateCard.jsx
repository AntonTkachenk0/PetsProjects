import catImage from '../assets/cat.jpg'; 

export default function createCard({name, breed, gender}){

    return (
        <>
        <div className='pet-card'>
                <div className='pet-avatar'>
                    <img src={catImage}/>
                </div>

                <div>
                    <h3>{name}</h3>
                    <p>Пол: {gender}</p>
                    <p>Порода: {breed}</p>

                </div>

            </div>
        </>
    );

}
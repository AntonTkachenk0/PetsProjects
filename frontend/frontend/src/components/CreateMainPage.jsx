import { useEffect, useState } from 'react';
import { fetchPets } from '../services/pets';
import CreateCard from './CreateCard'; 
export default function createMainPage(){

    const [pets, setPets] = useState([]);

    useEffect(() => {

        const loadData = async () => {
            var data =  await fetchPets();
            console.log(data);
            setPets(data);
        };
        loadData();
    }, []);

    return (
            <>
              <div className="pets-container"> 
            <span>Мои питомцы</span>
              {pets.map((pet) => (
                <CreateCard
                  key={pet.id}
                  pet={pet}
                />
              ))}
            </div>
            </>
    );
}

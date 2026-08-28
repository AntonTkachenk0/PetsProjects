import { useEffect, useState } from "react";
import { fetchService } from "../services/service";
import { fetchUsers } from "../services/users";
import { fetchPets } from "../services/pets";

export default function CreateServiceJournalForm({ onCreate }) {


    const [services, setServices] = useState([]);
    const [pets, setPets] = useState([]);
    const [appointment, setAppointment] = useState(null);

    function handleSubmit(e){
        e.preventDefault();
        onCreate(appointment);
    }

    useEffect(() => {

        const fetchData = async () => {

            let dataServices = await fetchService();
            let dataPets = await fetchPets();
            console.log(dataServices);
            console.log(dataPets);

            setServices(dataServices);
            setPets(dataPets);
        }

        fetchData();
    }, [])


    return (
        <>
            <div className='serviceJournal-form'>
                <form onSubmit={handleSubmit}>
                    <select
                        className="serviceJournal-input"
                        name="service"
                        id="service"
                        onChange={(e) => setAppointment({ ...appointment, id_Service: (e.target.value) })}
                    >
                        <option value="">Выберите услугу</option>
                        {services.map(service => (
                            <option key={service.id} value={service.id}>
                                {service.title}
                            </option>
                        ))}
                    </select>

                    <select
                        className="serviceJournal-input"
                        name="users"
                        id="users"
                        onChange={(e) => setAppointment({ ...appointment, id_Pet: (e.target.value) })}
                    >
                        <option value="">Выберите питомца</option>
                        {pets.map(pet => (
                            <option key={pet.id} value={pet.id}>
                                {pet.name}
                            </option>
                        ))}
                    </select>
                    <label for="localdate">Время: </label>
                    <input type="datetime-local" id="localdate" name="date"
                     onChange={(e) => setAppointment({ ...appointment, VisitDate:e.target.value})}
                    />

                    <button type="submit">отправить</button>
                </form>
            </div>
        </>
    );

}
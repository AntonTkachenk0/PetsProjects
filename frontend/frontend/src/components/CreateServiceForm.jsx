import { useState } from "react";

   
   export default function createServiceForm( {onCreate}) {
  
    const [service, setService] = useState(null);

    function handleSubmit(e) {

        const serviceToSend = {
            ...service, 
            cost: parseFloat(service.cost)
        }
        e.preventDefault();
        onCreate(service);
        setService(null);
    }
    
    return (
    <form className="service-form" onSubmit={handleSubmit}>
      <h3>Регистрация услуги</h3>
      <input 
      className="service-input" 
      placeholder='Введите название услуги ' 
      onChange={(e) => setService({...service, title: e.target.value})} />
      <input 
      className="service-input" 
      type="number"
        step="0.01"
       placeholder='Введите стоимость'
        onChange={(e) => setService({...service, cost:  parseFloat(e.target.value)})} />
      <button 
      className="service-button"
       type="submit">Отправить</button>
    </form>
    );
   }
  
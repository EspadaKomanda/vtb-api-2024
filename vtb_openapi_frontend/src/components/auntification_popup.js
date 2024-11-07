import React, { useState } from 'react';
import auntificationStore from '@/stores/auntification_store.js';
import EmailConfirmation from '@/components/email_confirmation_component.js';

const AuntificationPopup = ({ type }) => {
  const closeLogin = auntificationStore((state) => state.closeLogin);
  const closeRegister = auntificationStore((state) => state.closeRegister);
  const [isConfirmationOpen, setConfirmationOpen] = useState(false);
  const [isRegisterStepTwo, setRegisterStepTwo] = useState(false);

  const handleClose = () => {
    if (type === 'login') closeLogin();
    if (type === 'register') closeRegister();
  };

  const handleRegisterSubmit = (e) => {
    e.preventDefault();
    console.log('Регистрация успешна, открываем подтверждение почты');
    setConfirmationOpen(true);
  };

  const handleInitialRegisterSubmit = (e) => {
    e.preventDefault();
    setRegisterStepTwo(true);
  };

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white rounded-lg p-6 shadow-lg">
        <h2 className="text-lg font-bold mb-4">{type === 'login' ? 'Вход' : 'Регистрация'}</h2>
        {isConfirmationOpen ? (
          <EmailConfirmation />
        ) : type === 'login' ? (
          <LoginForm />
        ) : isRegisterStepTwo ? (
          <RegisterStepTwo onSubmit={handleRegisterSubmit} />
        ) : (
          <RegisterForm onSubmit={handleInitialRegisterSubmit} />
        )}
        <button onClick={handleClose} className="mt-4 text-red-500">Закрыть</button>
      </div>
    </div>
  );
};

const LoginForm = () => {
  return (
    <form>
      <input type="text" placeholder="Логин" className="border p-2 mb-2 w-full" />
      <input type="password" placeholder="Пароль" className="border p-2 mb-2 w-full" />
      <button type="submit" className="bg-blue-500 text-white p-2 rounded">Войти</button>
    </form>
  );
};

const RegisterForm = ({ onSubmit }) => {
  return (
    <form onSubmit={onSubmit}>
      <input type="text" placeholder="Логин" className="border p-2 mb-2 w-full" />
      <input type="email" placeholder="Почта" className="border p-2 mb-2 w-full" />
      <input type="password" placeholder="Пароль" className="border p-2 mb-2 w-full" />
      <button type="submit" className="bg-blue-500 text-white p-2 rounded">Далее</button>
    </form>
  );
};

const RegisterStepTwo = ({ onSubmit }) => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [middleName, setMiddleName] = useState('');
    const [birthDate, setBirthDate] = useState('');
    const [error, setError] = useState('');
  
    const handleSubmit = (e) => {
      e.preventDefault();
      const age = calculateAge(birthDate);
      if (age < 18) {
        setError('Вы должны быть старше 18 лет.');
        return;
      }
      setError('');
      onSubmit(e);
    };
  
    const calculateAge = (dateString) => {
      const birthDate = new Date(dateString);
      const today = new Date();
      let age = today.getFullYear() - birthDate.getFullYear();
      const monthDifference = today.getMonth() - birthDate.getMonth();
      if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
      return age;
    };
  
    return (
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Имя"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          className="border p-2 mb-2 w-full"
        />
        <input
          type="text"
          placeholder="Фамилия"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          className="border p-2 mb-2 w-full"
        />
        <input
          type="text"
          placeholder="Отчество"
          value={middleName}
          onChange={(e) => setMiddleName(e.target.value)}
          className="border p-2 mb-2 w-full"
        />
        <label className="block mb-2">Дата рождения:</label>
        <input
          type="date"
          value={birthDate}
          onChange={(e) => setBirthDate(e.target.value)}
          className="border p-2 mb-2 w-full"
          max={new Date().toISOString().split("T")[0]}
        />
        {error && <p className="text-red-500 mb-2">{error}</p>}
        <button type="submit" className="bg-blue-500 text-white p-2 rounded">Завершить регистрацию</button>
      </form>
    );
  };
  
  

export default AuntificationPopup;


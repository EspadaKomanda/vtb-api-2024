import { useState } from 'react';
import auntificationStore from '@/stores/auntification_store.js';
import EmailConfirmation from '@/components/email_confirmation_component.js';
import Image from 'next/image';
import * as img from '../assets/images.js';
import { motion } from 'framer-motion';
import Cookies from 'js-cookie';


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
      <div className="relative bg-custom-bg-gray lg:w-1/4 sm:w-1/2 w-11/12 text-white rounded-lg p-6 shadow-lg">
        <h2 className="text-2xl font-bold mb-4">{type === 'login' ? 'Вход' : 'Регистрация'}</h2>
        {isConfirmationOpen ? (
          <EmailConfirmation />
        ) : type === 'login' ? (
          <LoginForm onClose={handleClose}/>
        ) : isRegisterStepTwo ? (
          <RegisterStepTwo onSubmit={handleRegisterSubmit} />
        ) : (
          <RegisterForm onSubmit={handleInitialRegisterSubmit} />
        )}
        <button onClick={handleClose} className={isConfirmationOpen ? 'hidden' : 'absolute top-6 right-6'}>
          <Image 
            src={img.exit} 
            alt="close" 
            width={30}
            height={30} 
            className='transition duration-300 hover:scale-110 active:scale-95' 
          />
        </button>
      </div>
    </div>
  );
};


const LoginForm = ({ onClose }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!username || !password) {
      setError('Пожалуйста, заполните все поля.');
      return;
    }
    setError('');
    setIsLoading(true);

    try {
      const response = await fetch('', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        throw new Error('Ошибка входа');
      }

      const data = await response.json();
      console.log('Вход выполнен', data);
      onClose();
    } catch (error) {
      setError('Ошибка входа. Пожалуйста, проверьте свои данные.');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: -5 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.3, ease: 'easeInOut' }}
    >
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Логин"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="border p-2 mb-4 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="border p-2 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        {error && <p className="text-red-500 mb-2">{error}</p>}
        <button type="submit" className="bg-custom-bg-blue px-5 font-semibold text-white p-2 rounded mt-8" disabled={isLoading}>
          {isLoading ? 'Секунду..' : 'Войти'}
        </button>
      </form>
    </motion.div>
  );
};


const RegisterForm = ({ onSubmit }) => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [checkLogin, setCheckLogin] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const loginRegex = /^[a-zA-Z0-9_]{3,18}$/;
    if (!loginRegex.test(username)) {
      setError('Логин должен быть от 3 до 18 символов, использовать можно только латинские буквы, нижнее подчеркивание и цифры.');
      return;
    }
    if (!username || !email || !password) {
      setError('Пожалуйста, заполните все поля.');
      return;
    }
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
      setError('Пожалуйста, введите корректную почту.');
      return;
    }
    if (password.length < 8) {
      setError('Пароль должен быть не короче 8 символов.');
      return;
    }
    setError('');
    setCheckLogin(true);

    try {
      const response = await fetch('', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, email, password }),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Ошибка регистрации, попробуйте другой логин.');
      }

      const data = await response.json();
      console.log('Логин свободен', data);
      onSubmit(e);
    } catch (error) {
      setError(`Не удалось зарегистрироваться, попробуй другой логин (${error.message})`);
    } finally {
      setCheckLogin(false);
    }
    onSubmit(e); //TODO remove this function on release, temporary for checking
  };

  return (
    <motion.div
    initial={{ opacity: 0, y: -5  }}
    animate={{ opacity:  1,  y: 0 }}
    transition={{ duration: 0.3, ease: 'easeInOut' }}
    >
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Логин"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          className="border p-2 mb-4 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        <input
          type="email"
          placeholder="Почта"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        
          className="border p-2 mb-4 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        <input
          type="password"
          placeholder="Пароль"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="border p-2 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        {error && <p className="text-red-500 mb-2">{error}</p>}
        <button type="submit" className="bg-custom-bg-blue px-5 font-semibold text-white p-2 rounded mt-8" disabled={checkLogin}>{checkLogin ? 'Секунду..' : 'Продолжить'}</button>
      </form>
  </motion.div>
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
    if (!firstName || !lastName || !middleName || !birthDate) {
      setError('Пожалуйста, заполните все поля.');
      return;
    }
    const age = calculateAge(birthDate);
    if (age < 18) {
      setError('Вы должны быть старше 18 лет.');
      return;
    }
    setError('');

    Cookies.set('firstName', firstName, { sameSite: 'None', secure: true });
    Cookies.set('lastName', lastName, { sameSite: 'None', secure: true });
    Cookies.set('middleName', middleName, { sameSite: 'None', secure: true });
    Cookies.set('birthDate', birthDate, { sameSite: 'None', secure: true });
    
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
    <motion.div
    initial={{ opacity: 0, y: -5  }}
    animate={{ opacity:  1,  y: 0 }}
    transition={{ duration: 0.3, ease: 'easeInOut' }}
    >
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Имя"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          className="border p-2 mb-2 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        <input
          type="text"
          placeholder="Фамилия"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          className="border p-2 mb-2 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        <input
          type="text"
          placeholder="Отчество"
          value={middleName}
          onChange={(e) => setMiddleName(e.target.value)}
          className="border p-2 mb-2 w-full bg-customColor1 rounded-md focus:outline-none border-none"
        />
        <label className="block mb-2">Дата рождения:</label>
        <input
          type="date"
          value={birthDate}
          onChange={(e) => setBirthDate(e.target.value)}
          className="border p-2 mb-2 w-full bg-customColor1 rounded-md focus:outline-none border-none"
          max={new Date().toISOString().split("T")[0]}
        />
        {error && <p className="text-red-500 mb-2">{error}</p>}
        <button type="submit" className="bg-custom-bg-blue px-5 font-semibold text-white p-2 rounded">Завершить регистрацию</button>
      </form>
    </motion.div>
  );
};

export default AuntificationPopup;

export default function MainFeatures() {
    return (
        <div className="relative mt-14 min-h-80">
            <div className="absolute inset-0 bg-custom-blur bg-opacity-50 backdrop-blur-sm rounded-md conteiner"></div>
            <ul className="relative z-10 grid grid-cols-3 conteiner py-8 text-center text-white text-3xl font-medium">
                <li className="grid grid-rows-2 gap-y-24">
                    <span className="font-black bg-clip-text bg-custom-gradient text-6xl relative">
                        Копилка
                        <span className="absolute left-1/2 transform -translate-x-1/2 bottom-[-40px] border-b border-white border w-1/2"></span>
                    </span>
                    
                    <span>
                        На выбранные туры с донатов на Rutube
                    </span>
                </li>
                <li className="grid grid-rows-2 gap-y-24">
                    <span className="font-black bg-clip-text bg-custom-gradient text-6xl relative">
                        Страхование
                        <span className="absolute left-1/2 transform -translate-x-1/2 bottom-[-40px] border-b border-white border w-1/2"></span>
                    </span>
                    <span>
                        Жизни и здоровья
                    </span>
                </li>
                <li className="grid grid-rows-2 gap-y-24">
                    <span className="font-black bg-clip-text bg-custom-gradient text-7xl relative">
                        +10%
                        <span className="absolute left-1/2 transform -translate-x-1/2 bottom-[-40px] border-b border-white border w-1/2"></span>
                    </span>
                    <span>
                        Кэшбек с прошлых путешествий
                    </span>
                </li>
            </ul>
        </div>
    );
}

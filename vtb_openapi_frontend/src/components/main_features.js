export default function MainFeatures() {
    return (
        <div className="relative mt-20 min-h-80">
            <div className="absolute inset-0 bg-custom-blur bg-opacity-70 backdrop-blur-sm rounded-md conteiner"></div>
            <ul className="relative z-10 grid grid-cols-1 lg:grid-cols-3 container py-8 text-center text-white text-2xl lg:text-3xl font-medium gap-y-5">
                <li className="grid grid-rows-2 gap-y-6 lg:gap-y-24">
                    <span className="font-black bg-clip-text bg-custom-gradient text-4xl lg:text-6xl relative">
                        Копилка
                        <span className="absolute left-1/2 transform -translate-x-1/2 bottom-[-20px] lg:bottom-[-40px] border-b border-white border w-1/2"></span>
                    </span>
                    
                    <span>
                        На выбранные туры с донатов на Rutube
                    </span>
                </li>
                <li className="grid grid-rows-2 gap-y-6 lg:gap-y-24">
                    <span className="font-black bg-clip-text bg-custom-gradient text-4xl lg:text-6xl relative">
                        Страхование
                        <span className="absolute left-1/2 transform -translate-x-1/2 bottom-[-20px] lg:bottom-[-40px] border-b border-white border w-1/2"></span>
                    </span>
                    <span>
                        Жизни и здоровья
                    </span>
                </li>
                <li className="grid grid-rows-2 gap-y-6 lg:gap-y-24">
                    <span className="font-black bg-clip-text bg-custom-gradient text-4xl lg:text-7xl relative">
                        +10%
                        <span className="absolute left-1/2 transform -translate-x-1/2 bottom-[-20px] lg:bottom-[-40px] border-b border-white border w-1/2"></span>
                    </span>
                    <span>
                        Кэшбек с прошлых путешествий
                    </span>
                </li>
            </ul>
        </div>
    );
}

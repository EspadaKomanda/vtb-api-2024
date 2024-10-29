"use client";
import "../styles/navigation_component_styles.css";
import Link from "next/link";
import {usePathname} from "next/navigation";
import { useEffect, useState } from "react";

export default function NavigationComponent() {
    const pathname = usePathname();
    const [currentPath, setCurrentPath] = useState(pathname);

    useEffect(() => {
        setCurrentPath(pathname); 
    }, [pathname]);

    return (
        <nav className="container">
            <ul className="flex flex-wrap gap-y-10 justify-evenly text-3xl font-bold text-white ">
                <li>
                    <Link href="/" className={currentPath === "/" ? "active" : ""}>
                        Главная
                    </Link>
                </li>
                <li>
                    <Link href="/search_tour" className={currentPath === "/search_tour" ? "active" : ""} >
                        Поиск по турам
                    </Link>
                </li>
                <li>
                    <Link href="/choice_dosug" className={currentPath === "/choice_dosug" ? "active" : ""}>
                        Выбрать досуг
                    </Link>
                </li>
                <li>
                    <Link href="/wish_list" className={currentPath === "/wish_list" ? "active" : ""}>
                        Желаемое
                    </Link>
                </li>
                <li>
                    <Link href="/profile" className={currentPath === "/profile" ? "active" : ""}>
                        Профиль
                    </Link>
                </li>
            </ul>
        </nav>
    );
}
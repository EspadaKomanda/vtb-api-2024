import NavigationComponent from "@/components/navigation_component";
import SearchTourComponent from "@/components/serch_tour_component";
export default function Search() {
    return (
        <div>
            <header className="pt-8 text-white">
                {/* TODO: Скорректировать цвет навигационной панели на этой странице */}
                <NavigationComponent/>
            </header>
            <main className="container p-5">
                <SearchTourComponent/>
            </main>
        </div>
    );
}
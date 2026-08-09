import Menu from '../components/menu';
import { Outlet } from 'react-router-dom';

const MainLayout = () => {
  return (
    <div className="wrapper">
        <header>
          <Menu />
        </header>

        <main>
          <Outlet />
        </main>
      </div>
  )
}

export default MainLayout
import { AiOutlineDashboard } from "react-icons/ai";
import { TbBuildingCommunity, TbChartBar, TbUsersGroup } from "react-icons/tb";
import { GrDocumentTest } from "react-icons/gr";
import { LuLogOut, LuMenu } from "react-icons/lu";
import { useAuth } from "./contexts/AuthContext";
import { useNavigate } from "react-router-dom";

export default function Menu() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate("/login", { replace: true });
  };

  return (
    <>
        <nav className="navigation">
          <div>
          <h1><a href="#" className="logo-text manrope-extra-bold">Mandat<span><br></br>Verkstaden</span></a></h1>
          
          {/* Mobile and tablet menu */}
          <button className="hamburger-menu" popoverTarget="hamburger" type="button" aria-label="Open menu"><LuMenu /></button>
          <ul popover="auto" id="hamburger" className="menu-popover">
            {/* <li><a href="#" className="nav-opt manrope-bold"><AiOutlineDashboard /> Översikt</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbBuildingCommunity /> Kommuner</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbUsersGroup /> Partier</a></li> */}
            <li><a href="/electionResults" className="nav-opt manrope-bold"><TbChartBar  /> Valresultat</a></li>
            {/* <li><a href="/scenario" className="nav-opt manrope-bold"><GrDocumentTest /> Scenarier</a></li>
            <li><button type="button" className="nav-opt manrope-bold" onClick={handleLogout}><LuLogOut /> Logga ut</button></li> */}
          </ul>

          {/* Desktop menu */}
          <ul className="sidebar-menu">
            {/* <li><a href="#" className="nav-opt manrope-bold"><AiOutlineDashboard /> Översikt</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbBuildingCommunity /> Kommuner</a></li>
            <li><a href="#" className="nav-opt manrope-bold"><TbUsersGroup /> Partier</a></li> */}
            <li><a href="/electionResults" className="nav-opt manrope-bold"><TbChartBar  /> Valresultat</a></li>
            {/* <li><a href="/scenario" className="nav-opt manrope-bold"><GrDocumentTest /> Scenarier</a></li> */}
            {/* <li><button type="button" className="nav-opt manrope-bold" onClick={handleLogout}><LuLogOut /> Logga ut</button></li> */}
          </ul>
          </div>
        </nav>
    </>
  )
}

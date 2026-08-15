
export const AccentButton = ({ btnText, onClick }) => {
  return (
        <>
            <button className="btn btn-accent" onClick={onClick}>{btnText || ""}</button>
        </>
  )
}

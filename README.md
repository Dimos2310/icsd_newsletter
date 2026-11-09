# ICSD Newsletter API 📰

[![.NET](https://github.com/Dimos2310/icsd_newsletter/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Dimos2310/icsd_newsletter/actions/workflows/dotnet.yml)
![.NET 6.0](https://img.shields.io/badge/.NET-6.0-512BD4?logo=dotnet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-336791?logo=postgresql)
![Entity Framework Core](https://img.shields.io/badge/EF%20Core-7.0-68217A)

Μια RESTful Web API πλατφόρμα διαχείρισης ηλεκτρονικού ενημερωτικού δελτίου (Newsletter) και ειδησεογραφικού περιεχομένου, αναπτυγμένη σε **ASP.NET Core (.NET 6)** με βάση δεδομένων **PostgreSQL** και **Entity Framework Core**.

Το σύστημα υποστηρίζει πλήρη ροή δημοσίευσης άρθρων (draft, υποβολή, έγκριση, δημοσίευση), ιεραρχική κατηγοριοποίηση σε θεματικές ενότητες (Topics), έλεγχο ταυτότητας και ρόλων μέσω custom token authorization, καθώς και διαχείριση χρηστών.

---

## 👥 Συντελεστές (Ομάδα Ανάπτυξης)

Ακαδημαϊκή εργασία στο **Τμήμα Μηχανικών Πληροφοριακών & Επικοινωνιακών Συστημάτων (ICSD)** - Πανεπιστήμιο Αιγαίου.

* **Κυριακίδης Δημοσθένης** (icsd19102)
* **Ζαγαλιώτη Φωτεινή** (icsd19054)
* **Γκατζιάς Σπύρος** (icsd19030)

---

## 🚀 Τεχνολογικό Stack

* **Γλώσσα & Framework:** C# / .NET 6.0 (ASP.NET Core Web API)
* **ORM:** Entity Framework Core 7 (Npgsql.EntityFrameworkCore.PostgreSQL)
* **Βάση Δεδομένων:** PostgreSQL
* **Επικύρωση Δεδομένων (Validation):** FluentValidation 11.7.1
* **Ασφάλεια / Κρυπτογράφηση:** MD5 Password Hashing & Custom Token-based Authorization
* **CI/CD:** GitHub Actions Workflow (.NET build)

---

## 🏗️ Αρχιτεκτονική & Βασικά Χαρακτηριστικά

### 1. Ρόλοι & Δικαιώματα Χρηστών
Το σύστημα υποστηρίζει τρεις διακριτούς ρόλους:
* **Admin (`admin`):** Διαχείριση χρηστών (ενεργοποίηση, απενεργοποίηση, ανάθεση ρόλων, προβολή όλων των εγγεγραμμένων χρηστών).
* **Επιμελητής (`epimelitis`):** Έγκριση ή απόρριψη άρθρων και θεματικών, δημοσίευση άρθρων, διαγραφή/τροποποίηση θεματικών, καθώς και συγγραφή άρθρων.
* **Δημοσιογράφος (`dimosiografos`):** Δημιουργία, ενημέρωση και υποβολή άρθρων και προτάσεων θεματικών προς έγκριση.

### 2. Κύκλος Ζωής Είδησης (News Lifecycle)
Τα άρθρα περνούν από τα παρακάτω διαδοχικά στάδια (Status):
```
[CREATED] (Σύνταξη) 
    │
    ▼ (Υποβολή από Δημοσιογράφο)
[APPLIED] (Σε αναμονή αξιολόγησης)
    │
    ├──► [REJECTED -> CREATED] (Απόρριψη από Επιμελητή για διορθώσεις)
    │
    ▼ (Έγκριση από Επιμελητή)
[ACCEPTED] (Εγκεκριμένο)
    │
    ▼ (Δημοσίευση από Επιμελητή)
[PUBLISHED] (Δημοσιευμένο)
```

### 3. Ιεραρχικές Θεματικές Ενότητες (Topics)
Υποστηρίζεται ιεραρχική δομή δέντρου θεμάτων μέσω της σχέσης `ParentTopic` (αυτο-συσχέτιση), επιτρέποντας υποκατηγορίες (π.χ. *Τεχνολογία -> Τεχνητή Νοημοσύνη*).

### 4. Custom Token Authorization Header
Η πιστοποίηση και εξουσιοδότηση γίνεται μέσω του custom HTTP header:
```http
exousiodotisi: <auth-token-guid>
```
Το token εκδίδεται κατά το login, ισχύει για 24 ώρες και ελέγχεται σε κάθε κλήση μέσω του attribute `[RoleAccess("role1", "role2", ...)]`.

---

## 📋 Endpoints του API

### 👤 Διαχείριση Χρηστών (`/api/user`)

| Μέθοδος | Endpoint | Ρόλοι / Πρόσβαση | Περιγραφή |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/user` | Public | Εγγραφή νέου χρήστη (`status: INACTIVE`). Απαιτείται ισχυρός κωδικός. |
| `GET` | `/api/user` | Public | Σύνδεση (Login). Επιστρέφει το auth token. |
| `PATCH` | `/api/user` | `dimosiografos`, `epimelitis` | Ενημέρωση στοιχείων τρέχοντος χρήστη. |
| `PATCH` | `/api/user/updatepassword` | `dimosiografos`, `epimelitis`, `admin` | Αλλαγή κωδικού (`?oldp=...&newp=...`). |
| `PATCH` | `/api/user/setrole` | `admin` | Ορισμός ρόλου χρήστη (`?id=...&role=...`). |
| `PATCH` | `/api/user/enable/{id}` | `admin` | Ενεργοποίηση λογαριασμού χρήστη (`ACTIVE`). |
| `PATCH` | `/api/user/disable/{id}` | `admin` | Απενεργοποίηση λογαριασμού χρήστη (`INACTIVE`). |
| `GET` | `/api/user/getusers` | `admin` | Λήψη λίστας όλων των χρηστών. |
| `PATCH` | `/api/user/godmode/{id}` | Public / Setup | Ορισμός χρήστη ως Admin και άμεση ενεργοποίησή του. |

> **Πολιτική Κωδικού Πρόσβασης:** Ο κωδικός πρέπει να έχει τουλάχιστον 8 χαρακτήρες, τουλάχιστον ένα κεφαλαίο γράμμα και τουλάχιστον έναν ειδικό χαρακτήρα (π.χ. `@, #, $, !, %, *`).

---

### 📰 Διαχείριση Ειδήσεων (`/api/news`)

| Μέθοδος | Endpoint | Ρόλοι / Πρόσβαση | Περιγραφή |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/news` | Public | Λήψη όλων των ειδήσεων με θέματα, σχόλια και συντάκτη. |
| `GET` | `/api/news/{newsId}` | `dimosiografos`, `epimelitis` | Λήψη συγκεκριμένης είδησης βάσει ID. |
| `GET` | `/api/news/search` | Public | Αναζήτηση ειδήσεων βάσει λέξεων-κλειδιών (`?searchw=word1&searchw=word2`). |
| `POST` | `/api/news` | `dimosiografos`, `epimelitis` | Δημιουργία νέας είδησης (κατάσταση: `CREATED`). Έλεγχος μοναδικότητας τίτλου. |
| `PATCH` | `/api/news/update/{id}` | `dimosiografos`, `epimelitis` | Ενημέρωση είδησης (μόνο εφόσον βρίσκεται σε κατάσταση `CREATED`). |
| `PATCH` | `/api/news/submit/{id}` | `dimosiografos`, `epimelitis` | Υποβολή είδησης προς αξιολόγηση (`CREATED` ➔ `APPLIED`). |
| `PATCH` | `/api/news/approve/{id}` | `epimelitis` | Έγκριση είδησης (`APPLIED` ➔ `ACCEPTED`). |
| `PATCH` | `/api/news/reject/{id}` | `epimelitis` | Απόρριψη είδησης πίσω στο προσχέδιο (`APPLIED` ➔ `CREATED`). |
| `PATCH` | `/api/news/publish/{id}` | `epimelitis` | Δημοσίευση είδησης (`ACCEPTED` ➔ `PUBLISHED`). |

---

### 🏷️ Διαχείριση Θεματικών Ενοτήτων (`/api/topic`)

| Μέθοδος | Endpoint | Ρόλοι / Πρόσβαση | Περιγραφή |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/topic` | Public | Λήψη όλων των θεμάτων ταξινομημένων κατά status και όνομα. |
| `GET` | `/api/topic/{topicId}` | `dimosiografos`, `epimelitis` | Λήψη θέματος βάσει ID. |
| `POST` | `/api/topic` | `dimosiografos`, `epimelitis` | Δημιουργία νέου θέματος (με προαιρετικό `ParentTopic`). |
| `PUT` | `/api/topic/{topicId}` | `dimosiografos`, `epimelitis` | Ενημέρωση ονόματος ή γονικού θέματος. |
| `PUT` | `/api/topic/approve/{topicId}` | `epimelitis` | Έγκριση θέματος (`ACCEPTED`). |
| `PUT` | `/api/topic/reject/{topicId}` | `epimelitis` | Απόρριψη και διαγραφή θέματος (εφόσον είναι `CREATED`). |
| `DELETE` | `/api/topic/{topicId}` | `epimelitis` | Διαγραφή θέματος. |

---

## ⚙️ Οδηγίες Εγκατάστασης & Εκτέλεσης

### Προαπαιτούμενα
* [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) (ή νεότερο)
* [PostgreSQL](https://www.postgresql.org/) (τοπικά ή σε Docker container)
* [Entity Framework Core Tools (`dotnet-ef`)](https://docs.microsoft.com/ef/core/cli/dotnet)

### 1. Κλωνοποίηση του Αποθετηρίου
```bash
git clone https://github.com/Dimos2310/icsd_newsletter.git
cd icsd_newsletter/NewsLetter
```

### 2. Ρύθμιση Connection String
Ανοίξτε το αρχείο `appsettings.json` (ή `appsettings.Development.json`) και προσαρμόστε τα στοιχεία σύνδεσης της PostgreSQL:
```json
{
  "ConnectionStrings": {
    "dataConnection": "User ID=postgres;Password=YOUR_PASSWORD;Server=localhost;Port=5432;Database=icsd_newsletter;Integrated Security=true;Pooling=true;"
  }
}
```

### 3. Εκτέλεση Migrations (Δημιουργία Βάσης Δεδομένων)
Εφαρμόστε τα έτοιμα migrations στη βάση δεδομένων:
```bash
dotnet ef database update
```

### 4. Εκκίνηση της Εφαρμογής
```bash
dotnet run
```
Η εφαρμογή ξεκινάει και ακούει στις προεπιλεγμένες διευθύνσεις (π.χ. `https://localhost:7141` και `http://localhost:5178`).

---

## 🧪 Παράδειγμα Ροής Χρήσης (Quick Start)

1. **Εγγραφή Χρήστη (Register):**
   ```http
   POST /api/user
   Content-Type: application/json

   {
     "Username": "dimos",
     "Password": "Password123!"
   }
   ```

2. **Ορισμός ως Διαχειριστής (Godmode / Bootstrap):**
   ```http
   PATCH /api/user/godmode/1
   ```

3. **Σύνδεση (Login):**
   ```http
   GET /api/user
   Content-Type: application/json

   {
     "Username": "dimos",
     "Password": "Password123!"
   }
   ```
   *Αποτέλεσμα:* Επιστρέφεται το Token (π.χ. `3fa85f64-5717-4562-b3fc-2c963f66afa6`).

4. **Κλήση εξουσιοδοτημένου endpoint (π.χ. Δημιουργία Είδησης):**
   ```http
   POST /api/news
   exousiodotisi: 3fa85f64-5717-4562-b3fc-2c963f66afa6
   Content-Type: application/json

   {
     "Title": "Παρουσίαση Νέου Εργαστηρίου ICSD",
     "Content": "Στο τμήμα ICSD εγκαινιάστηκε το νέο εργαστήριο υπολογιστικών συστημάτων..."
   }
   ```

---

## 📂 Δομή Έργου

```text
icsd_newsletter/
├── NewsLetter.sln
├── .github/
│   └── workflows/
│       └── dotnet.yml            # GitHub Actions CI Workflow
└── NewsLetter/
    ├── Controllers/              # Web API Controllers (User, News, Topic, Comment)
    ├── Models/                   # Data Entities, DTOs & Enums
    │   ├── Context/              # EF Core DataContext
    │   └── DataTransferObjects/  # Filter & Transfer Options
    ├── Services/                 # Business Logic & DB Interactions
    ├── Authorization/            # RoleAccess & Header Action Filters
    ├── Validators/               # FluentValidation Rules
    ├── Migrations/               # EF Core DB Migrations
    ├── Properties/               # launchSettings.json
    ├── appsettings.json          # DB Connection & Logging Configuration
    └── Program.cs                # Entry point & Middleware Configuration
```

---

## 📄 Άδεια Χρήσης

Αυτό το έργο αναπτύχθηκε για εκπαιδευτικούς σκοπούς στα πλαίσια σπουδών στο Πανεπιστήμιο Αιγαίου (Τμήμα Μηχανικών Πληροφοριακών και Επικοινωνιακών Συστημάτων).
